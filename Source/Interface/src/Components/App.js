import React from 'react'
import { Switch, Route } from 'react-router-dom'
import { geocodeByAddress, getLatLng } from 'react-places-autocomplete'
import { arrayMove } from 'react-sortable-hoc'
import toastr from 'toastr'

import { getGeoLocation } from '../html5'
import mockData from '../Mock/routes'
import { Search, Research } from '../Services/SearchService'
import AddressManagerPage from './AddressManagerPage'
import RouteViewerPage from './RouteViewerPage';
import Header from './Header'

const format = 'HH:mm';
export default class App extends React.Component {
    constructor() {
        super()
        this.state = { 
            loading: false,
            reloading: [],
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
    }

    componentDidMount()
    {
       this.setToCurrentLocation()
    }

    setToCurrentLocation = () => 
        getGeoLocation()
        .then(location => this.setState({
            lat: location.coords.latitude,
            lng: location.coords.longitude
        }))
        .catch( e => {console.log(e); toastr.error(JSON.stringify(e))})
    
    
    onChange = (address) => this.setState({ address })
    onSelectPlace = location => this.setState({ ...location })
    onClickButton = location => this.setState({address:""})

    onHandleSelect = async address => {
        this.setState({ address })
        toastr.info(JSON.stringify(address))
        let latLng = await geocodeByAddress(address)
                                .then(results => getLatLng(results[0]))
                                .catch( e => {console.log(e); toastr.error(JSON.stringify(e))})
        this.setState({ ...latLng })
    }
    onChangeFrom = value => {
        const from = value ? value.format(format) : "00:00"
        this.setState({ from })
    }
    onChangeTo = value => {
        const to =  value ? value.format(format) : "00:00"
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
        this.setState({ loading: true, hasResults: true })
        window.scrollTo(0, 0)
        this.props.history.push('/result')
        try {
            toastr.info("Enviado...");
          const response = await Search(this.state.listLocations);
            this.setState({ 
                loading: false,
                results: response
            })
        }
        catch (e) {
            this.setState({ loading: false, hasResults:false })
            toastr.error(e);
        }
    }
    timeout = (ms)=> {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
    
    research = async (index, locations, time) => 
    {
        // Não tem mais destinos para calcular
        if(locations.length <= 1){
            toastr.info("Não a mais destinos...");
            return
        }

        let reloading = this.state.reloading
        reloading[index] = true

        this.setState({ reloading })
        toastr.info("Saindo "+ time +" Entregador " + (index + 1) + "...");
        await this.timeout(3000);

        //window.scrollTo(0, 0)
        //this.props.history.push('/result')
        try {
          //const response = await Research(locations);
          
          //results.rotas[index] = response.rotas
          reloading[index] = false
          this.setState({ reloading })
          toastr.info("Conluído Entregador " + (index + 1) + "...");
        }
        catch (e) {
            reloading[index] = false
            this.setState({ reloading })
            toastr.info("Erro Entregador " + (index + 1) + "\n" + e);
        }
    }

    render() {
        const state = this.state;
        let { results, loading, reloading } = state;

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
        
        const routePage = () => 
            <RouteViewerPage 
                loading={loading} 
                reloading={this.state.reloading} 
                results={results} 
                research={this.research} />

        return (
            <div className="app">
                <Header hasResults={this.state.hasResults}  />
                <Switch>
                    <Route exact path='/'  render={addressPage}/>
                    <Route path='/result' render={routePage}/>
                </Switch>
            </div>
        )
    }
}
