import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Hospitals } from './components/Hospitals'
import { HospitalsRunAndGo } from './components/HospitalsRunAndGo'
import { CovidTestingByState } from './components/CovidTestingByState'
import { CovidTestingByDate } from './components/CovidTestingByDate'


import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/testing-by-date' component={CovidTestingByDate} />
                <Route path='/testing-by-state' component={CovidTestingByState} />
                <Route path='/hospitals-run-go' component={HospitalsRunAndGo} />
                <Route path='/hospitals' component={Hospitals} />
            </Layout>
        );
    }
}
