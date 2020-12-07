import React, { Component } from 'react';

export class Hospitals extends Component {

    constructor(props) {
        super(props);
        this.state = { hospitals: [], isLoading: true };
    }

    componentDidMount() {
        this.loadCovidData();
    }

    renderHospitalsTable(hospitals) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>State</th>
                        <th>Type</th>
                        <th>ZipCode</th>
                        <th>Licensed Beds</th>
                        <th>Staffed Beds</th>
                        <th>Potential Increase in Beds</th>
                    </tr>
                </thead>
                <tbody>
                    {hospitals.map(hospitalItem =>
                        <tr key={hospitalItem.name}>
                            <td>{hospitalItem.name}</td>
                            <td>{hospitalItem.stateName}</td>
                            <td>{hospitalItem.hospitalType}</td>
                            <td>{hospitalItem.hqZipCode}</td>
                            <td>{hospitalItem.licencedBeds}</td>
                            <td>{hospitalItem.staffedBeds}</td>
                            <td>{hospitalItem.potentialIncreaseInBedCapac}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.isLoading
            ? <p><em>Loading...</em></p>
            : this.renderHospitalsTable(this.state.hospitals);

        return (
            <div>
                <h1 id="tabelLabel" >Hospitals Beds avaliability</h1>
                <p>This component demonstrates fetching COVID-19 data from the server that uses Amazon Athena to run SQL Standard query on S3 files from a Data Lake account. This Request run Athena Query and Wait for Results</p>
                {contents}
            </div>
        );
    }

    async loadCovidData() {
        const response = await fetch('covidtracking/hospitals');
        const data = await response.json();
        this.setState({ hospitals: data, isLoading: false });
    }
}