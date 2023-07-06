$(function () {
    function searchEmployee(searchQuery) {
        $("caption").html("Search results");
        $("tbody").html("");

        $.ajax({
            type: "POST",
            url: "/api/Search",
            data: "\"" + searchQuery + "\"",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (employees) {
                $.each(employees, function (i, employee) {
                    $("tbody").append($("<tr>"));
                    appendElement = $("tbody tr").last();
                    appendElement.append($("<td>").html('<p><a href="/app/employeedetails/' + employee.employeeId + '" class="link-dark text-decoration-none">' + employee.firstName + " " + employee.lastName + '</a></p>'));
                    appendElement.append($("<td>").html('<p>' + employee.position + '</p>'));
                    appendElement.append($("<td>").html('<p><a href="/app/locationdetails/' + employee.location.locationId + '" class="link-dark text-decoration-none">' + employee.location.locationName + '</a></p>'));
                });
            },
            error: function (xhr, status, error) {
                console.log(xhr)
            }
        });
    }

    $("#txtEmployeeName").on('keyup', function () {
        var searchQuery = $.trim($("#txtEmployeeName").val());

        searchEmployee(searchQuery);

    });

    $("#searchButton").on('click', function () {
        var searchQuery = $.trim($("#txtEmployeeName").val());

        searchEmployee(searchQuery);
    });
});