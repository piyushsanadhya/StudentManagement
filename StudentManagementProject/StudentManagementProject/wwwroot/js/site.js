// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Ajax helpers
$.ajaxGet = function (url, success) {
    return $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: success
    });
};

$.ajaxPost = function (url, data, success) {
    data.__RequestVerificationToken = $("[name='__RequestVerificationToken']").val();

    var ajax = {
        url: url,
        type: 'POST',
        dataType: 'json',
        data: data,
        headers: {
            'X-VerificationToken': $("[name='__RequestVerificationToken']").val()
        },
        success: success
    };

    return $.ajax(ajax);
};