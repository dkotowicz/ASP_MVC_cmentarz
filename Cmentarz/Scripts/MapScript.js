var sektorPopup = "<b>Podaj nazwę sektora:</b><br/><input id=\"sektorNameInput\" type=\"text\"/><input type=\"button\" value=\"Zapisz\"  onClick=\"SaveSektorName()\"/>";
var sektorPopupOptions = {
	maxWidth: 400,
	width: 200,
	fontSize: 18
};

var markerPopup = "<b>Podaj opis markera:</b><br/><input id=\"markerDescriptionInput\" type=\"text\"/><input type=\"button\" value=\"Zapisz\"  onClick=\"SaveMarkerDesc()\"/>";
var markerPopupOptions = {
	maxWidth: 400,
	maxHeight: 800,
	width: 200,
	fontSize: 18,
	offset: new L.Point(0, -20)
};

var myMarker = L.Icon.extend({
	options: {
		shadowUrl: null,
		iconAnchor: new L.Point(12, 40),
		iconSize: new L.Point(24, 40),
		iconUrl: "/Content/images/marker-icon.png"
	}
});

var map = new L.Map("map", {
	crs: L.CRS.Simple,
	minZoom: -2,
	maxZoom: 0
});

var sidebar = L.control.sidebar("sidebar", {
	position: "left",
	closeButton: true,
	autoPan: true
});

var currentSector = null;
var currentMarker = null;
var drawnItems = new L.FeatureGroup();

function init() {
	if ($("#map").length <= 0)
		return;

	var bounds = [[0, 0], [1440, 1920]];
	L.imageOverlay("/Content/Map/map.png", bounds).addTo(map);
	map.fitBounds(bounds);

	drawnItems = new L.GeoJSON.AJAX("/Content/data.json", {
		onEachFeature: function(feature, layer) {
			if (layer instanceof L.Polygon) {
				layer.setStyle(dim);
				layer.on("click", searchSektor).on("mouseover", highlightRect).on("mouseout", dimRect);
			}

			if (layer instanceof L.Marker)
				layer.bindPopup(layer.feature.properties.description);
		}
	});

	map.addLayer(drawnItems);

	//var drawControl = new L.Control.Draw({
	//	position: "topleft",
	//	draw: {
	//		polyline: false,
	//		polygon: false,
	//		circle: false,
	//		rectangle: {
	//			shapeoptions: {
	//				clickable: true
	//			}
	//		},
	//		marker: {
	//			icon: new myMarker()
	//		}
	//	},
	//	edit: {
	//		featureGroup: drawnItems,
	//		remove: true
	//	}
	//});
	//map.addControl(drawControl);

	map.addControl(sidebar);
};

function getRzadList() {
	//debugger;
	var stateId = $("#sektorDropDown").val();
	$.ajax
	({
		url: '/Kwatera/GetRzadList',
		type: 'POST',
		datatype: 'application/json',
		contentType: 'application/json',
		data: JSON.stringify({
			stateId: +stateId
		}),
		success: function (result) {
			$("#rzadDropDown").html("");
			$("#rzadDropDown").append($('<option></option>').val("").html("Znalezione rzędy"));
			$.each($.parseJSON(result), function (i, city)
			{ $("#rzadDropDown").append($('<option></option>').val(city.rzad).html(city.rzad)) })
		}
	});
}

map.on("contextmenu", function () {
	return;
});

map.on("draw:created", function (e) {
	var type = e.layerType,
		layer = e.layer;

	if (type === "rectangle") {
		layer.addTo(drawnItems).on("click", searchSektor).on("mouseover", highlightRect).on("mouseout", dimRect);
		layer.bindPopup(sektorPopup, sektorPopupOptions);
		layer.openPopup();

		currentSector = layer;
	}

	if (type === "marker") {
		layer.addTo(drawnItems).on("click", setThisMarker);
		layer.bindPopup(markerPopup, markerPopupOptions);
		layer.openPopup();

		currentMarker = layer;
	}

	drawnItems.addLayer(layer);
});

//$("#save").click(function () {
//	var data = { json: JSON.stringify(drawnItems.toGeoJSON()) }

//	$.ajax({
//		type: "post",
//		dataType: "json",
//		url: "Home/SaveMap",
//		data: data,
//		success: function(result) {
//			if (result)
//				alert("Zapisano zmiany");
//			else
//				alert("Nie udało się zapisać zmian");
//		}
//	});
//});

function ShowSidebar() {
	if(!sidebar.isShown)
		sidebar.show();
};

function SaveSektorName() {
	var feature = currentSector.feature = currentSector.feature || {};
	feature.type = "Feature";
	feature.properties = feature.properties || {};
	feature.properties.sektorName = document.getElementById("sektorNameInput").value;

	currentSector.closePopup();
	currentSector.unbindPopup();
	currentSector = null;
};

function SaveMarkerDesc() {
	currentMarker.closePopup();
	currentMarker.unbindPopup();

	var feature = currentMarker.feature = currentMarker.feature || {};
	feature.type = "Feature";
	feature.properties = feature.properties || {};
	feature.properties.description = document.getElementById("markerDescriptionInput").value;

	currentMarker.bindPopup(feature.properties.description, { offset: new L.Point(0, -20) });
	currentMarker = null;
};

function searchSektor() {
	currentSector = this;
	$("#search-form").trigger("reset");
	var sektorId = document.getElementById("sektorDropDown");
	for (var i = 0; i < sektorId.length; i++) {
		if (sektorId[i].text === this.feature.properties.sektorName) {
			sektorId[i].selected = true;
			break;
		}
	}
	getRzadList();
	setTimeout(function () {
		$("#submit").click();
	}, 1000);
}

function setThisMarker() {
	currentMarker = this;
}

function highlightRect() {
	this.setStyle(highlight);
};

function dimRect() {
	this.setStyle(dim);
};

var highlight = {
	color: "#848fb4",
	stroke: false
};

var dim = {
	color: "#FFFFFF",
	stroke: false
};