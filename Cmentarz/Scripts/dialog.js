function CustomAlert(){
    this.render = function(dialog){
        var winW = window.innerWidth;
        var winH = window.innerHeight;
        var dialogoverlay = document.getElementById('dialogoverlay');
        var dialogbox = document.getElementById('dialogbox');
        dialogoverlay.style.display = "block";
        dialogoverlay.style.height = winH+"px";
        dialogbox.style.left = (winW/2) - (550 * .5)+"px";
        dialogbox.style.top = "100px";
        dialogbox.style.display = "block";
        document.getElementById('dialogboxhead').innerHTML = "Uwaga";
        document.getElementById('dialogboxbody').innerHTML = dialog;
        document.getElementById('dialogboxfoot').innerHTML = '<button class="btn btn-default" onclick="Alert.ok()">Ok</button>';
        document.getElementById('model').style.display = "none";
    }
	this.ok = function(){
		document.getElementById('dialogbox').style.display = "none";
		document.getElementById('dialogoverlay').style.display = "none";
		document.getElementById('model').style.display = "block";
	}
}
var Alert = new CustomAlert();


function CustomConfirm() {
    this.render = function (dialog, op, id) {
        var winW = window.innerWidth;
        var winH = window.innerHeight;
        var dialogoverlay = document.getElementById('dialogoverlay');
        var dialogbox = document.getElementById('dialogbox');
        dialogoverlay.style.display = "block";
        dialogoverlay.style.height = winH + "px";
        dialogbox.style.left = (winW / 2) - (550 * .5) + "px";
        dialogbox.style.top = "100px";
        document.getElementById('model').style.display = "none";
        dialogbox.style.display = "block";

        document.getElementById('dialogboxhead').innerHTML = "Usuwanie osoby";
        document.getElementById('dialogboxbody').innerHTML = dialog;
        document.getElementById('dialogboxfoot').innerHTML = '<button class="btn btn-default" onclick="Confirm.yes(\'' + op + '\',\'' + id + '\')">Tak</button> <button class="btn btn-default" onclick="Confirm.no()">Nie</button>';
    }
    this.no = function () {
        document.getElementById('dialogbox').style.display = "none";
        document.getElementById('dialogoverlay').style.display = "none";
        document.getElementById('model').style.display = "block";
    }
    this.yes = function (op, id) {
        if (op == "delete_post") {
            
            window.location = "/osoba/delete/" + id;
        }
        document.getElementById('dialogbox').style.display = "none";
        document.getElementById('dialogoverlay').style.display = "none";
        document.getElementById('model').style.display = "block";
    }
}
var Confirm = new CustomConfirm();

