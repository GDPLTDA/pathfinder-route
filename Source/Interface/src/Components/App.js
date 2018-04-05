import React from 'react'
import PlaceSearch from './PlaceSearch'
import Map from './Map'
import { getGeoLocation } from '../html5'
import AdressList from './AddressList'
import SearchRoute from './SearchRoute'
import LoadTest from './LoadTest'
import TableRoute from './TableRoute'
import LoadingSpinner from './LoadingSpinner'
import { geocodeByAddress, getLatLng } from 'react-places-autocomplete'
import toastr from 'toastr'
import {arrayMove} from 'react-sortable-hoc'

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

    UpdateStoreStatus(locations = []){
        return locations.map(  
            (entry,index) =>
               (index === 0)
               ? {...entry, isStore: true}
               : {...entry, isStore: false}
            )
    }

    GetDados = ()=> 
    this.setState({
        listLocations: [
        { 
            address: 'Rua Maria Roschel Schunck, 817',
            isStore: true, 
            lat:0,
            lng:0,
            from: '06:00', 
            to: '23:00',
            wait: 30
        },
        {
            address: 'Av. das Nações Unidas, 22540',
            isStore: false,
            lat:0,
            lng:0,
            from: '08:00', 
            to: '10:00',
            wait: 30 
        },
        { 
            address: 'Rua Urussuí, 271 - Itaim Bibi, São Paulo - SP, Brasil',
            isStore: false,
            lat:0,
            lng:0,
            from: '12:00', 
            to: '12:00',
            wait: 90 
        },
        { 
            address: 'Av. Paulista - Bela Vista, São Paulo - SP, Brasil',
            isStore: false,
            lat:0,
            lng:0,
            from: '12:00', 
            to: '15:00',
            wait: 30 
        },
        { 
            address: 'Rua Augusta - Consolação, São Paulo - SP, Brasil',
            isStore: false,
            lat:0,
            lng:0,
            from: '12:00', 
            to: '23:00',
            wait: 20 
        },
        { 
            address: 'Av. Engenheiro Eusébio Stevaux, 823',
            isStore: false, 
            lat:0,
            lng:0,
            from: '12:00', 
            to: '13:00',
            wait: 60 
        },
        { 
            address: 'Rua Vergueiro - Vila Dom Pedro I, São Paulo - SP, Brasil',
            isStore: false,
            lat:0,
            lng:0,
            from: '12:00', 
            to: '20:00',
            wait: 10 
        },
        { 
            address: 'Praça da Sé - Centro, São Paulo - SP, Brasil',
            isStore: false,
            lat:0,
            lng:0,
            from: '12:00', 
            to: '23:00',
            wait: 30 
        },
        { 
            address: 'Catavento Cultural e Educacional - Avenida Mercúrio - Brás, São Paulo - SP, Brasil',
            isStore: false,
            lat:0,
            lng:0,
            from: '12:00',
            to: '17:00',
            wait: 30
        }
    ]})

    Search = async (e) => {
        let items = this.state.listLocations
        let store = items.find(function (obj) { return obj.isStore; });
        let listDestinos = items.filter(function (obj) { return !obj.isStore; });
        let destinos = []

        for (let i = 0; i < listDestinos.length; i++) {
            destinos.push(
                {
                    Endereco : listDestinos[i].address,
                    DhInicial : listDestinos[i].from + ":00",
                    DhFinal : listDestinos[i].to + ":00",
                    MinutosEspera : 30,
                    
                }
            )
        } 
        let json = {
            DhSaida : "11/12/2017 " + store.from + ":00",
            DhLimite : "11/12/2017 " + store.to + ":00",
            Origem :{
                Endereco : store.address
            },
            Destinos : destinos
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
                    <LoadTest Teste={this.GetDados}/>
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
                {
                    loading ? <LoadingSpinner /> : 
                    <TableRoute mensagem={results[0].mensagem} listEntregador = {results}/>
                }
            </div>
        )
    }
}
