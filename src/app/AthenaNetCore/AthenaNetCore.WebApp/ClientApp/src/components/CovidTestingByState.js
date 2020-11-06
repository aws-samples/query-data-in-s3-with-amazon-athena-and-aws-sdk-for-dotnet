/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: MIT-0
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify,
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
import React, { Component } from 'react'
import data from '../data/states.json'

export class CovidTestingByState extends Component {

    constructor(props) {
        super(props);
        this.state = {
            selectedValue: "--",
            covidTesting: [],
            isLoading: true,
            isCheckingStatus: false,
            statusData: {
                atempt: 0,
                queryId: ""
            }
        };

        this.stateSelectChange = this.stateSelectChange.bind(this);
    }

    stateSelectChange(event) {
        const selectedState = event.target.value;
        //console.log(selectedState);
        if (selectedState !== "--") {
            this.setState({ isLoading: true });
            this.loadCovidData(selectedState);
        } else {
            this.setState({ covidTesting: [] });
        }
    }


    renderGridTable(covidTestingResults) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>State</th>
                        <th>Positive</th>
                        <th>Negative</th>
                        <th>Pending</th>
                        <th>Hospitalized</th>
                        <th>Death</th>
                        <th>Positive Increase</th>
                    </tr>
                </thead>
                <tbody>
                    {covidTestingResults.map(item =>
                        <tr key={item.date + item.state}>
                            <td>{item.date}</td>
                            <td>{item.state}</td>
                            <td>{item.positive}</td>
                            <td>{item.negative}</td>
                            <td>{item.pending}</td>
                            <td>{item.hospitalized}</td>
                            <td>{item.death}</td>
                            <td>{item.positiveincrease}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    renderStatusCheck(statusData) {
        return (
            <p><em>Running query ID: {statusData.queryId}, Atemp: {statusData.atempt}</em></p>
        );
    }

    render() {
        let contents = this.state.isLoading
            ? (this.state.isCheckingStatus ? this.renderStatusCheck(this.state.statusData) : <p><em>You need to select one State...</em></p>)
            : this.renderGridTable(this.state.covidTesting);

        return (
            <div>
                <h1 id="tabelLabel" >Testing Progress by State</h1>
                <p>This component demonstrates fetching COVID-19 data from the server that uses Amazon Athena to run SQL Standard query on S3 files from a Data Lake account. This Component run Athena Query, get QueryExecutionId, check status of execution, and list results </p>
                <select onChange={this.stateSelectChange} value={this.state.selectedValue} >
                    <option value="--">-Select one option-</option>
                    {
                        data.map(state =>
                            <option key={state.abbreviation} value={state.abbreviation} >{state.name}</option>)
                    }
                </select>
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
                    atempt: this.state.statusData.atempt + 1
                }
            });
            this.scheduleStatusCheck(queryId);
        } else {
            clearInterval(this.timer);
            await this.loadResult(queryId);
        }
    }

    async loadResult(queryId) {
        const response = await fetch(`/covidtracking/testing/run/result/${queryId}`);
        const dataResult = await response.json();
        this.setState({ covidTesting: dataResult, isLoading: false });
    }

    async loadCovidData(stateAbbreviation) {
        const response = await fetch(`/CovidTracking/testing/run/${stateAbbreviation}`);
        const dataResult = await response.json();
        this.setState({
            isCheckingStatus: true,
            statusData: {
                queryId: dataResult.queryId,
                atempt: 1
            }
        });
        await this.scheduleStatusCheck(dataResult.queryId);
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