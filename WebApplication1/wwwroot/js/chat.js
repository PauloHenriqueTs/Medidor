"use strict";

jQuery(document).ready(function ($) {
    var errorSignalr = $("#signalr")

    errorSignalr.hide();
    var value = $("#jwt").val();

    var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5001/chathub",
        { accessTokenFactory: () => value }
    ).build();

    connection.on("ErrorMessages", function (message) {
        console.log(message)
        var json = JSON.parse(message)
        if (json.Type === 2) {
            errorSignalr.show()
            console.log(`Error Switch Command ${json.serialId}`)
            errorSignalr.text(`Error Switch Command ${json.serialId}`)
        }
    });

    connection.start().then(function () {
        connection.invoke("JoinGroup").catch(function (err) {
            return console.error(err.toString());
        });
    }).catch(function (err) {
        return console.error(err.toString());
    });
});