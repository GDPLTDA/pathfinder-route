import React from 'react'
import { Switch, Route } from 'react-router-dom'
import { geocodeByAddress, getLatLng } from 'react-places-autocomplete'
import { arrayMove } from 'react-sortable-hoc'
import toastr from 'toastr'

import { getGeoLocation } from '../html5'
import mockData from '../Mock/routes'
import {Search} from '../Services/SearchService'
import AddressManagerPage from './AddressManagerPage'
import RouteViewerPage from './RouteViewerPage';

const format = 'HH:mm';
export default class App extends React.Component {
    constructor() {
        super()
        this.state = { 
            loading: false, 
            address: '',
            results: [], 
            lat: 0, 
            lng: 0, 
            isStore: true, 
            from: '00:00', to: '00:00', 
            wait: 30,
            hasResults: false,
            listLocations: [] 
        }

        this.setToCurrentLocation()
    }

    setToCurrentLocation = async () => {
        const location = await getGeoLocation()
        this.setState({
            lat: location.coords.latitude,
            lng: location.coords.longitude
        })
    }
    
    onChange = (address) => this.setState({ address })
    onSelectPlace = location => this.setState({ ...location })
    onClickButton = location => this.setState({address:""})

    handleSelect = async address => {
        this.setState({ address })
        toastr.error(JSON.stringify(address))
        let latLng = await geocodeByAddress(address)
                                .then(results => getLatLng(results[0]))
                                .catch( e => {console.log(e); toastr.info(JSON.stringify(e))})

        this.setState({ ...latLng })
    }
    onChangeFrom = value => {
        const from = value.format(format)
        this.setState({ from })
    }
    onChangeTo = value => {
        const to = value.format(format)
        this.setState({ to })
    }
    onChangeWait = value => {
        const wait = value
        this.setState({ wait })
    }

    onRemoveLocation = (item) => {
        this.setState({
            listLocations: this.UpdateStoreStatus(this.state.listLocations)
        })
    }

    onSortEnd = (oldIndex, newIndex) => {
        this.setState({
            listLocations: this.UpdateStoreStatus(
                arrayMove(this.state.listLocations, oldIndex, newIndex))
            });
    };

    updateStoreStatus = (locations = []) =>
        locations.map((entry,index) => ({...entry, isStore: (index === 0)}))

    getDados = () => this.setState(mockData)

    search = async () => {
        this.setState({ loading: true })
        try {
        //   const response = await Search(this.state.listLocations);
        //     this.setState({ 
        //         loading: false,
        //         results: response,
        //         hasResults: true
        //     })
            this.props.history.push('/result')
            
        }
        catch (e) {
            this.setState({ loading: false, hasResults:false })
            toastr.error(e);
        }
    }

    render() {
        const state = this.state;
        let { results, loading } = state;

        if(results.length !== 0){
            if(typeof results.mensagem !== 'undefined'){
                console.log(results.mensagem)
                results=[{mensagem: results.mensagem, rotas:[]}]
            }
        }
        else
            results=[{mensagem: "", rotas:[]}]
 
        
        const addressPage = () =>
            <AddressManagerPage 
                onSelect={this.onSelect}
                onHandleSelect={this.onHandleSelect}
                onChangeFrom={this.onChangeFrom}
                onChangeTo={this.onChangeTo}
                onChangeWait={this.onChangeWait}
                valueWait={state.wait}
                onTextChange={this.onChange}
                format={format}
                address={state.address}
                onRemoveLocation={this.onRemoveLocation}
                onSortEnd={this.onSortEnd}
                listLocations={state.listLocations}
                location={{...state}}
                onClickButton={this.onClickButton}
                getDados={this.getDados}
                search={this.search}
                lat={state.lat}
                lng={state.lng}
            />
        
        const routePage = () => <RouteViewerPage loading={loading} results={results} />

        return (
            <div className="app">
                <Switch>
                    <Route exact path='/'  component={addressPage}/>
                    <Route path='/result' component={routePage}/>
                </Switch>
            </div>
        )
    }
}
