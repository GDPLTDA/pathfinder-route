import React from 'react'
import PlaceSearch from './PlaceSearch'
import Map from './Map'
import { getGeoLocation } from '../html5'
import AdressList from './AddressList'
import SearchRoute from './SearchRoute'
import TableRoute from './TableRoute'
import LoadingSpinner from './LoadingSpinner'
import { geocodeByAddress, getLatLng } from 'react-places-autocomplete'
import toastr from 'toastr'
import {arrayMove} from 'react-sortable-hoc'

const format = 'HH:mm';
export default class App extends React.Component {
    constructor() {
        super()
        this.state = { loading: false, address: '',results: [], lat: 0, lng: 0, isStore: true, from: '00:00', to: '00:00', listLocations: [] }
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

    onRemoveLocation = (item) => {
        this.setState({
            istLocations: this.UpdateStoreStatus(this.state.listLocations)
        })
    }

    onSortEnd = (oldIndex, newIndex) => {
        this.setState({
            listLocations: this.UpdateStoreStatus(
                arrayMove(this.state.listLocations, oldIndex, newIndex))
            });
    };

    UpdateStoreStatus(locations = []){
        return locations.map(  
            (entry,index) =>
               (index === 0)
               ? {...entry, isStore: true}
               : {...entry, isStore: false}
            )
    }

    Search = async (e) => {
        let items = this.state.listLocations
        let store = items.find(function (obj) { return obj.isStore; });
        let listDestinos = items.filter(function (obj) { return !obj.isStore; });

        console.log(store)
        console.log(listDestinos)

        let json = {
          DhSaida : "11/12/2017 10:00:00",
          DhLimite : "12/12/2017 10:00:00",
          Origem :{
                  Endereco : "Rua Maria Roschel Schunck, 817"
          },
          Destinos : [
                {
                    Endereco : "Av. Engenheiro Eusébio Stevaux, 823",
                    DhInicial : "12:00:00",
                    DhFinal : "23:00:00",
                    MinutosEspera : 30
                },
                {
                    Endereco : "Av. das Nações Unidas, 22540",
                    DhInicial : "12:00:00",
                    DhFinal : "23:00:00",
                    MinutosEspera : 30
                },
                {
                    Endereco : "Rua Urussuí, 271 - Itaim Bibi, São Paulo - SP, Brasil",
                    DhInicial : "12:00:00",
                    DhFinal : "23:00:00",
                    MinutosEspera : 30
                },
                {
                    Endereco : "Av. Paulista - Bela Vista, São Paulo - SP, Brasil",
                    DhInicial : "12:00:00",
                    DhFinal : "23:00:00",
                    MinutosEspera : 30
                },
                {
                    Endereco : "Rua Augusta - Consolação, São Paulo - SP, Brasil",
                    DhInicial : "12:00:00",
                    DhFinal : "23:00:00",
                    MinutosEspera : 30
                },
                {
                    Endereco : "Rua Vergueiro - Vila Dom Pedro I, São Paulo - SP, Brasil",
                    DhInicial : "12:00:00",
                    DhFinal : "23:00:00",
                    MinutosEspera : 30
                },
                {
                    Endereco : "Praça da Sé - Centro, São Paulo - SP, Brasil",
                    DhInicial : "12:00:00",
                    DhFinal : "23:00:00",
                    MinutosEspera : 30
                },
                {
                    Endereco : "Catavento Cultural e Educacional - Avenida Mercúrio - Brás, São Paulo - SP, Brasil",
                    DhInicial : "12:00:00",
                    DhFinal : "23:00:00",
                    MinutosEspera : 30
                }
            ]
        }
        this.setState({
            loading: true
        })
        const response =  await fetch('http://localhost:64880/api/route', {
            method: 'POST',
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(json)
          })
          .then((response) => response.json())
          .catch( e => {console.log(e); toastr.info(JSON.stringify(e))})
          
          this.setState({
            loading: false,
            results: response
        })
    }

    render() {
        const state = this.state
        const { results, loading } = this.state;

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
                        onRemoveLocation={this.onRemoveLocation}
                        onSortEnd={this.onSortEnd}
                        items={this.state.listLocations}
                        location={{ ...state }}
                        onClickButton={this.onClickButton}
                    />
                    <SearchRoute Search={this.Search}/>
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
                {loading ? <LoadingSpinner /> : <TableRoute listEntregador ={results} />}
                
            </div>
        )
    }
}
