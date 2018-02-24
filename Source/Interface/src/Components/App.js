import React from 'react'
import PlaceSearch from './PlaceSearch'
import Map from './Map'
import { getGeoLocation } from '../html5'
import AdressList from './AddressList'
import SearchRoute from './SearchRoute'
import { geocodeByAddress, getLatLng } from 'react-places-autocomplete'
import toastr from 'toastr'

const format = 'HH:mm';
export default class App extends React.Component {
    constructor() {
        super()
        this.state = { address: '', lat: 0, lng: 0, isStore: true, from: '00:00', to: '00:00' }
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
        let latLng = await geocodeByAddress(this.state.address)
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
    render() {
        const state = this.state

        return (
            <div className="row app">
                <div className="col-sm-5">
                    <PlaceSearch
                        onSelect={this.onSelectPlace}
                        onHandleSelect={this.handleSelect}
                        onChangeFrom={this.onChangeFrom}
                        onChangeTo={this.onChangeTo}
                        onTextChange={this.onChange}
                        format={format}
                        address={this.state.address}
                    />
                    <AdressList
                        location={{ ...state }}
                        onClickButton={this.onClickButton}
                    />
                    <SearchRoute />
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
            </div>
        )
    }
}
