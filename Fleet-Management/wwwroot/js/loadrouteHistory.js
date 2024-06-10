document.addEventListener('DOMContentLoaded', function () {
    const apiUrl = 'https://localhost:7062/api/RouteHistory/GetAll';

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // تحديث جدول Route History
            const routeHistories = data.DicOfDT.RouteHistories;
            const tableBody = document.querySelector('#routeHistoryTable tbody');
            tableBody.innerHTML = '';

            if (Array.isArray(routeHistories) && routeHistories.length > 0) {
                routeHistories.forEach(item => {
                    const row = document.createElement('tr');
                    row.innerHTML = `
                        <td>${item.VehicleID}</td>
                        <td>${item.VehicleNumber}</td>
                        <td>${item.Address}</td>
                        <td>${item.Status}</td>
                        <td>${item.Latitude}</td>
                        <td>${item.Longitude}</td>
                        <td>${item.VehicleDirection}</td>
                        <td>${item.GPSSpeed}</td>
                        <td>${item.GPSTime}</td>
                    `;
                    tableBody.appendChild(row);
                });
            } else {
                const row = document.createElement('tr');
                row.innerHTML = `<td colspan="9">No route history data available.</td>`;
                tableBody.appendChild(row);
            }

            // تحديث بيانات JSON في جدول المقارنة
            const jsonDataDisplay = document.querySelector('#jsonDataDisplay');
            jsonDataDisplay.textContent = JSON.stringify(data, null, 2);
        })
        .catch(error => {
            console.error('Fetch error:', error);
            alert('Error fetching route history data.');
        });
});