"use strict";

$(function () {

    function init() {

        /* Prepare generic stuff.
         */
        if (templateGuid != "undefined") {
            doGenerate(templateGuid);
        }

        if (exporters != "undefined") {
            doAddExporters(exporters);
        }

        /* Set up event handlers.
         */
        $("#lnkDeleteReport").click(onDeleteReport);
    }

    function doAddExporters(exporters) {

        var $list = $("#export");

        $.each(exporters, function (prop, item) {

            var $li = $("<li>");
            $li.append("<a href='/Report/DownloadReport?guid=" + templateGuid + "&format=" + prop + "'>" + item + "</a>");

            $li.appendTo($list);
        });
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