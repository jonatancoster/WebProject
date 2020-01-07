var app = angular.module('myApp', []);

var apiUrl = 'http://localhost:55748/Api/Panels/';



app.controller('myCtrl', function ($scope, $http) {

    // The html class names for the squares. The state of a square is used as an index to get the corresponding class name.
    $scope.htmlClasses = ["state0", "state1", "state2"];

    // This is the panel that is rendered on screen. By default each square has state 0.
    $scope.panel = {
        name: "panel",
        squares: []
    };
    for (var i = 0; i < 36; i++) {
        $scope.panel.squares.push({ state: 0 });
    }

    /**
     * @description Retrieves the names of the panels stored on the server.
     */
    $scope.getPanels = function () {
        $http({
            method: 'GET',
            url: apiUrl,
            withCredentials: true
        }).then(function successCallback(response) {
            console.log("Retrieved panels:" + '\n' + response.data);
            $scope.panelNames = response.data;
        }, function errorCallback(response) {
                console.error("An error occurred while retreiving panels");
        });
    }

    /**
     * @description Retrieves the panel with the specified name from the server.
     * @param {string} panelName The name of the panel to retrieve.
     */
    $scope.getPanel = function (panelName) {
        var panelUrl = encodeURI(apiUrl + panelName);
        $http({
            method: 'GET',
            url: panelUrl,
            withCredentials: true
        }).then(function successCallback(response) {
            console.log("Retrieved panel from: " + panelUrl);
            $scope.panel.name = response.data.Name;
            for (var i = 0; i < response.data.Squares.length; i++) {
                $scope.panel.squares[i].state = response.data.Squares[i].State;
            }
        }, function errorCallback(response) {
                console.error("An error occurred while retreiving panel from " + panelUrl);
        });
    }

    /**
     * @description Stores the currently displayed panel on the server. 
     *              If the panel already exists, the user is asked if it should be replaced.
     */
    $scope.savePanel = function () {
        $http({
            method: 'POST',
            url: apiUrl,
            withCredentials: true,
            data: $scope.panel
        }).then(function successCallback(response) {
            console.log("Saved panel: " + $scope.panel.name);
            $scope.getPanels();
        }, function errorCallback(response) {
            console.error("An error occurred while saving panel " + $scope.panel.name);
            if (response.status == 409) {
                if (confirm("A configuration with this name already exist. Do you wish to overwrite it?")) {
                    $scope.replacePanel();
                }
            }
        });
    }

    /**
     * @description Deletes a panel with the specified name from the server.
     * @param {any} panelName The name of the panel to delete.
     */
    $scope.deletePanel = function (panelName) {
        var panelUrl = encodeURI(apiUrl + panelName);
        $http({
            method: 'DELETE',
            url: panelUrl,
            withCredentials: true
        }).then(function successCallback(response) {
            console.log("Deleted panel at: " + panelUrl);
            $scope.getPanels();
        }, function errorCallback(response) {
            console.error("An error occurred while deleting panel at " + panelUrl);
        });
    }

    /**
     * @description Replaces a panel which is already stored on the server.
     */
    $scope.replacePanel = function () {
        var panelUrl = encodeURI(apiUrl + $scope.panel.name);
        $http({
            method: 'PUT',
            url: panelUrl,
            withCredentials: true,
            data: $scope.panel
        }).then(function successCallback(response) {
            console.log("Replaced panel: " + $scope.panel.name);
            $scope.getPanels();
        }, function errorCallback(response) {
            console.error("An error occurred while replacing panel " + $scope.panel.name);
        });
    }



    $scope.getPanels();
});














