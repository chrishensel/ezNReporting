"use strict";

$(function () {

    function init() {

        /* Prepare generic stuff.
         */
        if (templateGuid != "undefined") {
            doGenerate(templateGuid);
        }

        /* Set up event handlers.
         */
        $("#lnkDeleteReport").click(onDeleteReport);
    }

    function doGenerate(templateGuid) {

        $.getJSON("/Report/GetGeneratedReport", { guid: templateGuid }, function (data) {
            var $result = $("#result");
            var $duration = $("#duration");

            if (data.success === false) {
                $result.html("Error! " + data.error);
                $duration.html("-");
            } else {
                $result.html(data.html);
                $duration.html(data.duration);
            }
        });
    }


    function onDeleteReport() {

        var data = {
            guid: templateGuid
        };

        $.ajax({
            url: "/Report/Delete",
            data: data,
            type: "POST",
            success: function (data) {
                window.location.href = "/Home";
            }
        });

    }

    init();
});