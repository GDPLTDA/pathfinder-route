import React from 'react'
import { geocodeByAddress, getLatLng } from 'react-places-autocomplete'
import {arrayMove} from 'react-sortable-hoc'
import toastr from 'toastr'

import PlaceSearch from './PlaceSearch'
import Map from './Map'
import { getGeoLocation } from '../html5'
import AdressList from './AddressList'
import SearchRouteButton from './SearchRouteButton'
import TableRoute from './TableRoute'
import LoadingSpinner from './LoadingSpinner'
import mockData from '../Mock/routes'
import {Search} from '../Services/SearchService'

const format = 'HH:mm';
export default class App extends React.Component {
    constructor() {
        super()
        this.state = { loading: false, address: '',results: [], lat: 0, lng: 0, isStore: true, from: '00:00', to: '00:00', wait: 30, listLocations: [] }
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

    UpdateStoreStatus = (locations = []) =>
        locations.map((entry,index) => ({...entry, isStore: (index === 0)}))

    GetDados = () => this.setState(mockData)

    Search = async () => {
        this.setState({ loading: true })
        try {
          const response = await Search(this.state.listLocations);
            this.setState({ 
                loading: false,
                results: response
            })
        }
        catch (e) {
            this.setState({ loading: false })
            toastr.error(e);
        }
    }

    render() {
        const state = this.state
        let { results, loading } = this.state;

        if(results.length !== 0){
            if(typeof results.mensagem !== 'undefined'){
                console.log(results.mensagem)
                results=[{mensagem: results.mensagem, rotas:[]}]
            }
        }
        else
            results=[{mensagem: "", rotas:[]}]
            
        return (
            <div className="form-group row app">
                <div className="form-group col-sm-5">
                    <PlaceSearch
                        onSelect={this.onSelectPlace}
                        onHandleSelect={this.handleSelect}
                        onChangeFrom={this.onChangeFrom}
                        onChangeTo={this.onChangeTo}
                        onChangeWait={this.onChangeWait}
                        ValueWait={this.state.wait}
                        onTextChange={this.onChange}
                        format={format}
                        address={this.state.address}
                    />
                    <AdressList
                        onRemoveLocation={this.onRemoveLocation}
                        onSortEnd={this.onSortEnd}
                        items={this.state.listLocations}
                        location={{ ...state }}
                        onClickButton={this.onClickButton}
                        Teste={this.GetDados}
                    />
                    <SearchRouteButton Search={this.Search}/>
                </div>
                <div className="col-sm-7">
                    <Map
                        lat={state.lat}
                        lng={state.lng}
                        loadingElement={<div style={{ height: `100%` }} />}
                        containerElement={<div style={{ height: `100%` }} />}
                        mapElement={<div style={{ height: `100%` }} />}
                    />
                </div>
                {
                    loading ? <LoadingSpinner /> : 
                    <TableRoute mensagem={results[0].mensagem} listEntregador = {results}/>
                }
            </div>
        )
    }
}
