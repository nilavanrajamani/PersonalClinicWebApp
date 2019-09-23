$(document).ready(function () {
    $('.summernote').summernote();
    resourceOperations.edit();
});

var resourceOperations = (function () {
    var obj = new Object();

    //private functions
    var isValid = function () {
        var isValid = true;
        var titleText = $('#resourceTitle').val();
        if (!titleText.trim()) {
            toastr.error('Please enter any title', 'ERROR!')
            isValid = false;
        }
        return isValid;
    };

    var saveComplete = function (result) {
        if (result) {
            if (result.IsSuccess) {                
                swal({
                    title: "Success",
                    text: result.Message,
                    type: "success"
                }, function () {
                    window.location.href = globalVariables.GetAllResourceLocation;
                });
            }
            else {
                toastr.error(result.Message, 'ERROR!');
                swal({
                    title: "ERROR!",
                    text: result.Message,
                    type: "error"
                });
            }
        }
    }


    //public methods
    obj.edit = function () {
        $('.click2edit').summernote({ focus: true });
    };
    obj.save = function () {
        if (isValid()) {
            var aHTML = $('.click2edit').summernote('code'); //save HTML If you need(aHTML: array).
            globalEvents.HandlePost('/Resource/AddUpdateResource', { title: $('#resourceTitle').val(), htmlContentToSave: aHTML, resourceId: $('#hdnResourceId').val() }, saveComplete, null, null);
        }
    };
    return obj;
})();
