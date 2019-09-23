var globalEvents = (function () {
    var returnObj = new Object();
    var showLoading = function () {
        $('body').addClass('sk-loading');
    };
    var hideLoading = function () {
        $('body').removeClass('sk-loading');
    };
    returnObj.HandlePost = function (url, inputParameters, successCallBack, additionalParams, errorCallBack) {
        var deferred = $.Deferred();
        showLoading();

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            type: 'POST',
            url: url,
            data: JSON.stringify(inputParameters),
            success: function (result) {
                successCallBack(result, additionalParams);
            },
            complete: function () {
                deferred.resolve();
                hideLoading();
            },
            error: function (req, status, error) {
                toastr.error('SERVER ERROR', 'ERROR!');
                errorCallBack();
            }
        });
    };
    return returnObj;
})();