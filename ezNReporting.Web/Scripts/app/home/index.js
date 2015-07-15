"use strict";

const iCount = 50;

$(function () {

    $.getJSON("/Report/GetReportsList", { lastId: 0, count: iCount }, getReportsListCallback);

    function getReportsListCallback(data) {

        var $parent = $("#list");

        var $table = $("<table><thead><tr><td>ID</td><td>Name</td><td>Created at</td><td>Author</td></tr></thead></table>");
        var $tbody = $("<tbody>");

        $.each(data.reports, function (i, report) {

            var $row = $("<tr></tr>");
            var date = new Date(report.CreatedAt);

            $row.append("<td>" + report.Id + "</td>");
            $row.append("<td>" + "<a href='/Home/Details?guid=" + report.Guid + "' target='__blank'>" + report.Name + "</a></td>");
            $row.append("<td>" + date.toLocaleDateString() + " " + date.toLocaleTimeString() + "</td>");
            $row.append("<td>" + report.CreatedBy + "</td>");

            $row.appendTo($tbody);
        });

        $tbody.appendTo($table);
        $table.appendTo($parent);
    }
});