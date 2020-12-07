import React, { Component } from 'react'

export class HospitalsRunAndGo extends Component {

    constructor(props) {
        super(props);
        this.state = {
            hospitals: [],
            isLoading: true,
            isCheckingStatus: false,
            statusData: {
                attempt: 0,
                queryId: ""
            }
        };
    }

    componentDidMount() {
        this.loadCovidData();
    }

    renderHospitalsTable(hospitals) {
        return (
            <table className='table table-striped' aria-labelledby="tableLabel">
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

    renderStatusCheck(statusData) {
        return (
            <p><em>Running query ID: {statusData.queryId}, Atemp: {statusData.attempt}</em></p>
        );
    }

    render() {
        let contents = ""
        if (this.state.isLoading) {
            contents = (this.state.isCheckingStatus ? this.renderStatusCheck(this.state.statusData) : <p><em>Loading...</em></p>);
        } else {
            contents = this.renderHospitalsTable(this.state.hospitals);
        }

        return (
            <div>
                <h1 id="tableLabel" >Hospitals Beds avaliability</h1>
                <p>This component demonstrates fetching COVID-19 data from the server that uses Amazon Athena to run SQL Standard query on S3 files from a Data Lake account. This Request run Athena Query and Wait for Results</p>
                {contents}
            </div>
        );
    }

    async checkQueryStatus(queryId) {
        const response = await fetch(`covidtracking/query/status/${queryId}`);
        const statusResult = await response.json();

        if (statusResult.isStillRunning) {
            this.setState({
                isCheckingStatus: true,
                isLoading: true,
                statusData: {
                    queryId: queryId,
                    attempt: this.state.statusData.attempt + 1
                }
            });
            this.scheduleStatusCheck(queryId);
        } else {
            clearInterval(this.timer);
            this.loadResult(queryId);
        }
    }

    async loadResult(queryId) {
        const response = await fetch(`/covidtracking/hospitals/run/result/${queryId}`);
        const dataResult = await response.json();
        this.setState({ hospitals: dataResult, isLoading: false });
    }

    async loadCovidData() {
        const response = await fetch('/covidtracking/hospitals/run');
        const dataResult = await response.json();
        this.setState({
            isCheckingStatus: true,
            statusData: {
                queryId: dataResult.queryId,
                attempt: this.state.statusData.attempt + 1
            }});
        this.scheduleStatusCheck(dataResult.queryId);
    }

    scheduleStatusCheck(queryId) {
        const SECONDS = 3;
        if (this.timer) {
            clearInterval(this.timer);
        }
        this.timer = setInterval(() => {
            this.checkQueryStatus(queryId);
        }, (SECONDS * 1000));
    }
}