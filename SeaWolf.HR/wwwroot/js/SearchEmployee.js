$(function () {
    function searchEmployee(searchQuery = "", sorter = "LastName") {

        if (searchQuery.length == 0) {
            $("caption").html("All employees");
        }
        else {
            $("caption").html("Search results");
        }

        $("tbody").html("");

        let values = searchQuery + "$$" + sorter

        $.ajax({
            type: "POST",
            url: "/api/Search",
            data: "\"" + values + "\"",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (employees) {
                $.each(employees, function (i, employee) {
                    $("tbody").append($("<tr>"));
                    appendElement = $("tbody tr").last();
                    appendElement.append($("<td>").html('<p><a href="/app/employeedetails/' + employee.employeeId + '" class="link-dark text-decoration-none">' + employee.lastName + ", " + employee.firstName + '</a></p>'));
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

        if (searchQuery.length > 1 || searchQuery.length == 0) {
            searchEmployee(searchQuery);
        }

    });

    $("#searchButton").on('click', function () {
        var searchQuery = $.trim($("#txtEmployeeName").val());

        if (searchQuery.length > 1 || searchQuery.length == 0) {
            searchEmployee(searchQuery);
        }
    });
    let nameCounter = 0;
    $("#nameSort").on('click', function () {
        var searchQuery = $.trim($("#txtEmployeeName").val());

        if (nameCounter == 0) {
            nameCounter++;
            searchEmployee(searchQuery);
        }
        else if (nameCounter >= 1) {
            nameCounter--;
            var sorter = "NameDesc";
            searchEmployee(searchQuery, sorter);
        }
        
    });
    let positionCounter = 0;
    $("#positionSort").on('click', function () {
        var searchQuery = $.trim($("#txtEmployeeName").val());
        var sorter = "Position";

        if (positionCounter == 0) {
            positionCounter++;
            searchEmployee(searchQuery, sorter);
        }
        else if (positionCounter == 1) {
            positionCounter--;
            sorter = "PositionDesc";
            searchEmployee(searchQuery, sorter);
        }
    });
    let locationCounter = 0;
    $("#locationSort").on('click', function () {
        var searchQuery = $.trim($("#txtEmployeeName").val());
        var sorter = "Location";

        if (locationCounter == 0) {
            locationCounter++;
            searchEmployee(searchQuery, sorter);
        }
        else if (locationCounter == 1) {
            locationCounter--;
            sorter = "LocationDesc";
            searchEmployee(searchQuery, sorter);
        }
    });
});