document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/VehicleInformation')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(responseData => {
            const data = responseData.$values;
            const tableBody = document.querySelector('#vehicleInfoTable tbody');
            if (Array.isArray(data)) {
                data.forEach(item => {
                    const row = document.createElement('tr');
                    row.innerHTML = `
                            <td>${item.vehicleID}</td>
                            <td>${item.driverName}</td>
                            <td>${item.vehicleMake}</td>
                            <td>${item.vehicleModel}</td>
                            <td>${new Date(item.purchaseDate).toLocaleDateString()}</td>
                        `;
                    tableBody.appendChild(row);
                });
            } else {
                console.error('Expected data to be an array, received:', data);
                alert('Data format error: Expected an array of vehicle information.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('There was an error fetching the vehicle data.');
        });
});
