"use strict";

$(function () {
    $("#txtName").focus();
});

function getInputTextOrDefault(elem, defaultValue) {

    var val = elem.val();

    if (val != "") {
        return val;
    }

    return defaultValue;
}

function submit() {

    var data = {
        name: getInputTextOrDefault($("#txtName"), "created from JS"),
        createdBy: getInputTextOrDefault($("#txtAuthor"), "Dummy")
    };

    $.ajax({
        url: "/Report/Create",
        data: data,
        type: "POST",
        success: function (sd) {
            window.location.href = "/Home/Details?guid=" + encodeURIComponent(sd.guid);
        }
    });
}