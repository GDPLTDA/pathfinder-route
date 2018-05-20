import React from 'react'
import { Switch, Route } from 'react-router-dom'
import { geocodeByAddress, getLatLng } from 'react-places-autocomplete'
import { arrayMove } from 'react-sortable-hoc'
import toastr from 'toastr'

import { getGeoLocation } from '../html5'
import mockData from '../Mock/DataMock'
import { Search, Research } from '../Services/SearchService'
import AddressManagerPage from './AddressManagerPage'
import RouteViewerPage from './RouteViewerPage';
import Header from './Header'

const format = 'HH:mm';
export default class App extends React.Component {
    constructor() {
        super()
        this.state = {
            selectedOptionTest: '',
            loading: false,
            reloading: [],
            address: '',
            results: [], 
            lat: 0, 
            lng: 0, 
            isStore: true, 
            from: '00:00', to: '00:00', 
            wait: 30,
            entregador: 1,
            hasResults: false,
            listLocations: [],

                generations: 1000,
                population: 100,
                mutation:   0,
                traffic :   0,
                useCache:   true
        }
    }

    componentDidMount()
    {
       this.setToCurrentLocation()
    }

    getGaConfig = () => ({
        generations: this.state.generations,
        population: this.state.population,
        mutation: this.state.mutation,
        traffic: this.state.traffic,
        useCache: this.state.useCache,
    })

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
    onChangeEntregador = value => {
        const entregador = value
        this.setState({ entregador })
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

    search = async () => {
        this.setState({ loading: true, hasResults: true })
        window.scrollTo(0, 0)
        this.props.history.push('/result')

        try {
            toastr.info("Enviado..." + this.state.selectedOptionTest.label);
            const response = await Search(this.state.selectedOptionTest.label, this.state.entregador, this.state.listLocations, this.getGaConfig());
            
            this.setState({ 
                loading: false,
                results: response
            })
        }
        catch (e) {
            this.setState({ loading: false, hasResults:false })
            toastr.error("Error" + e);
        }
    }
    timeout = (ms)=> {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
    
    SelectTestChange = (selectedOptionTest) => {
        this.setState({ selectedOptionTest });
        
        if(selectedOptionTest=== null){
            let mock = { listLocations: [] }
            this.setState(mock)
        }
        else{
            let index = selectedOptionTest.value;
            this.setState(mockData[index])
            console.log(`Selected: ${index}`);
        }
      }

    setConfig = (name) => (e) => 
    {
        switch (name) {
            case 'useCache':
                this.setState({useCache: e.target.checked})
                break;
        
            case 'traffic':
                this.setState({traffic: e && e.value})
                break;

            case 'mutation':
                this.setState({mutation: e && e.value})
                break;

            case 'generations':
                this.setState({generations: e.target.value})
                break;

            case 'population':
                this.setState({population: e.target.value})
                break;
            default:
                break;
        }
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

        try {
            const destinos = locations.map(
                d => ({
                    address: d.chegada.endereco,
                    from: d.chegada.dhInicial,
                    to: d.chegada.dhFinal,
                    isStore: false,
                    wait: d.chegada.minutosEspera
               })
            )

            let results = this.state.results;
            destinos[0].from = time;
            destinos[0].to = locations[0].saida.dhFinal;
            destinos[0].isStore = true;
            
            const response = await Research(destinos, this.getGaConfig());

            if(response.length !== 0){
                if(typeof response.mensagem !== 'undefined'){
                    results[index] = {mensagem: response.mensagem, rotas:locations}
                }
                else
                {
                    if(response.length === 1)
                        results[index] = response[0]
                    else
                    results[index] = {mensagem: "Não é possível terminar a entrega, para realizar a entrega seria preciso mais de 1 entregador!", rotas:locations}
                }
            }
            else 
            {
                results={mensagem: "", rotas:[]}
            }
            reloading[index] = false
            this.setState({ reloading, results })

            if(typeof response.mensagem !== 'undefined')
                toastr.error(response.mensagem + ", Entregador " + (index + 1) + "...");
            else
                toastr.info("Conluído Entregador " + (index + 1) + "...");
        }
        catch (e) {
            reloading[index] = false
            this.setState({ reloading })
            toastr.error("Erro Entregador " + (index + 1) + "\n" + e);
        }
    }

    render() {
        const state = this.state;
        let { results, loading } = state;
        const { selectedOptionTest } = state;

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
                onChangeEntregador={this.onChangeEntregador}
                SelectTestChange={this.SelectTestChange}
                SelectedOption={selectedOptionTest}
                ValueWait={state.wait}
                ValueEntregador={state.entregador}
                onTextChange={this.onChange}
                format={format}
                address={state.address}
                onRemoveLocation={this.onRemoveLocation}
                onSortEnd={this.onSortEnd}
                listLocations={state.listLocations}
                location={{...state}}
                setConfig={this.setConfig}
                onClickButton={this.onClickButton}
                search={this.search}
                lat={state.lat}
                lng={state.lng}
            />
        
        const routePage = () => 
            <RouteViewerPage 
                search={this.search}
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
