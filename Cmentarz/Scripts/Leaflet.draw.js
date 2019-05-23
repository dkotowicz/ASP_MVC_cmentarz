/**
 * Leaflet.draw assumes that you have already included the Leaflet library.
 */
L.drawVersion = '0.4.2';
/**
 * @class L.Draw
 * @aka Draw
 *
 *
 * To add the draw toolbar set the option drawControl: true in the map options.
 *
 * @example
 * ```js
 *      var map = L.map('map', {drawControl: true}).setView([51.505, -0.09], 13);
 *
 *      L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
 *          attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
 *      }).addTo(map);
 * ```
 *
 * ### Adding the edit toolbar
 * To use the edit toolbar you must initialise the Leaflet.draw control and manually add it to the map.
 *
 * ```js
 *      var map = L.map('map').setView([51.505, -0.09], 13);
 *
 *      L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
 *          attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
 *      }).addTo(map);
 *
 *      // FeatureGroup is to store editable layers
 *      var drawnItems = new L.FeatureGroup();
 *      map.addLayer(drawnItems);
 *
 *      var drawControl = new L.Control.Draw({
 *          edit: {
 *              featureGroup: drawnItems
 *          }
 *      });
 *      map.addControl(drawControl);
 * ```
 *
 * The key here is the featureGroup option. This tells the plugin which FeatureGroup contains the layers that
 * should be editable. The featureGroup can contain 0 or more features with geometry types Point, LineString, and Polygon.
 * Leaflet.draw does not work with multigeometry features such as MultiPoint, MultiLineString, MultiPolygon,
 * or GeometryCollection. If you need to add multigeometry features to the draw plugin, convert them to a
 * FeatureCollection of non-multigeometries (Points, LineStrings, or Polygons).
 */
L.Draw = {};

/**
 * @class L.drawLocal
 * @aka L.drawLocal
 *
 * The core toolbar class of the API — it is used to create the toolbar ui
 *
 * @example
 * ```js
 *      var modifiedDraw = L.drawLocal.extend({
 *          draw: {
 *              toolbar: {
 *                  buttons: {
 *                      polygon: 'Draw an awesome polygon'
 *                  }
 *              }
 *          }
 *      });
 * ```
 *
 * The default state for the control is the draw toolbar just below the zoom control.
 *  This will allow map users to draw vectors and markers.
 *  **Please note the edit toolbar is not enabled by default.**
 */
L.drawLocal = {
	// format: {
	// 	numeric: {
	// 		delimiters: {
	// 			thousands: ',',
	// 			decimal: '.'
	// 		}
	// 	}
	// },
	draw: {
		toolbar: {
			// #TODO: this should be reorganized where actions are nested in actions
			// ex: actions.undo  or actions.cancel
			actions: {
				title: 'Anuluj rysowanie',
				text: 'Anuluj'
			},
			finish: {
				title: 'Zakończ rysowanie',
				text: 'Zakończ'
			},
			undo: {
				title: 'Usuń ostatni narysowany punkt',
				text: 'Usuń ostatni punkt'
			},
			buttons: {
				polyline: 'Narysuj linię',
				polygon: 'Narysuj wielokąt',
				rectangle: 'Narysuj prostokąt',
				circle: 'Narysuj koło',
				marker: 'Narysuj marker'
			}
		},
		handlers: {
			circle: {
				tooltip: {
					start: 'Kliknij i przeciągnij, aby narysować koło.'
				},
				radius: 'Promień'
			},
			marker: {
				tooltip: {
					start: 'Naciśnij mapę, aby zamienić marker.'
				}
			},
			polygon: {
				tooltip: {
					start: 'Kliknij, aby zacząć rysować kształt.',
					cont: 'Kliknij, aby kontynuować rysowanie kształtu.',
					end: 'Kliknij pierwszy punkt, aby zamknąć kształt.'
				}
			},
			polyline: {
				error: '<strong>Error:</strong> krawędzie kształtu nie mogą się przecinać!',
				tooltip: {
					start: 'Kliknij, aby zacząć rysować linię.',
					cont: 'Kliknij, aby kontynuować rysowanie linii.',
					end: 'Kliknij ostatni punkt, aby zakończyć rysowanie linii.'
				}
			},
			rectangle: {
				tooltip: {
					start: 'Kliknij i przeciągnij, aby narysować prostokąt.'
				}
			},
			simpleshape: {
				tooltip: {
					end: 'Puść przycisk myszy, aby zakończyć rysowanie.'
				}
			}
		}
	},
	edit: {
		toolbar: {
			actions: {
				save: {
					title: 'Zapisz zmiany.',
					text: 'Zapisz'
				},
				cancel: {
					title: 'Przerwij edytowanie, anuluj wszystkie zmiany.',
					text: 'Anuluj'
				},
				clearAll:{
					title: 'Usuń wszystkie elementy.',
					text: 'Wyczyść wszystko'
				}
			},
			buttons: {
				edit: 'Edytuj element.',
				editDisabled: 'Brak elementów do edycji.',
				remove: 'Usuń element.',
				removeDisabled: 'Brak elementów do usunięcia.'
			}
		},
		handlers: {
			edit: {
				tooltip: {
					text: 'Przesuń uchwyty lub marker, aby edytować.',
					subtext: 'Kliknij anuluj, aby cofnąć zmiany.'
				}
			},
			remove: {
				tooltip: {
					text: 'Kliknij na kształt/marker, aby usunąć.'
				}
			}
		}
	}
};
