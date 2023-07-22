$(function () {
    var $deleteToggle = $("#delete-toggle");
    var $popupDeletionForm = $(".SW-delete-data");
    $popupDeletionForm.hide();

    $deleteToggle.on("click", function () {
        $popupDeletionForm.toggle();
    });
});