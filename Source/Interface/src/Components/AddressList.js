import React from 'react'
import _ from 'lodash'
import SortableList from './SortableList'
import Select from 'react-select'
import 'react-select/dist/react-select.css'

export default class AddressList extends React.Component
{

    addLocation = () => {
        const locations = this.props.items
        const newLocation = {...this.props.location}

        const found = locations.some( el =>
            el.lat === newLocation.lat 
            && el.lng === newLocation.lng
        )

        if ( newLocation.address==="" || found)
            return;

        if(locations.length !== 0)
            newLocation.isStore = false

        locations.push(newLocation)
        this.setState({listLocations: locations})

        this.props.onClickButton()
    }

    onRemoveLocation = (item) => {
        const items = this.props.items
        _.remove(items, item)
        this.props.onRemoveLocation(item)
    }

    onSortEnd = ({oldIndex, newIndex}) => {
        this.props.onSortEnd(oldIndex, newIndex)
    };

    render() {
        return (
            <div className="form-group search-bar">
                <div className="row">
                    <div className="col-md-10">
                        Teste<Select
                        value={this.props.SelectedOption}
                        onChange={this.props.SelectTestChange}
                        options={[
                        { value: 0, label: 'Teste para 1 Entregador' },
                        { value: 1, label: 'Teste para 2 Entregador' },
                        { value: 2, label: 'Teste para 3 Entregador' },
                        { value: 3, label: 'Teste para 4 Entregador' },
                        { value: 4, label: 'Teste não é possível' },
                        { value: 5, label: 'Teste não é possível com apenas 1' },
                        ]}
                        />
                    </div>

                    <div className="col-md-2">
                        <button className="btn btn-success" onClick={this.addLocation}>Adicionar</button>
                    </div>
                </div>
                <div className="row" >
                    <div className="checkbox col-md-5">
                        <label>
                            <input type="checkbox" onChange={this.props.setConfig('showConfig')} checked={this.props.location.showConfig} /> Configuração avançada
                        </label>
                    </div>
                </div>
                <div className="row form-group" hidden={!this.props.location.showConfig}>
                    <div className=" checkbox col-md-2">
                       <label><input type="checkbox" onChange={this.props.setConfig('useCache')} checked={this.props.location.useCache}/>Usar cache</label>
                    </div>
                    <div className="col-md-3">
                        <label>Tráfego:</label>
                        <Select
                        onChange={this.props.setConfig('traffic')}
                        value={this.props.location.traffic}
                        options={[
                        { value: 0, label: 'Média' },
                        { value: 1, label: 'Pessimista' },
                        { value: 2, label: 'Otimista' },
                        ]}
                        />
                    </div>
                    <div className="col-md-3">
                        <label>Mutação</label>
                        <Select
                        onChange={this.props.setConfig('mutation')}
                        value={this.props.location.mutation}
                        options={[
                        { value: 0, label: 'Swap' },
                        { value: 1, label: 'Inversion' },
                        { value: 2, label: 'Insertion' },
                        { value: 3, label: 'Displacement' },
                        ]}
                        />
                    </div>
                    <div className="col-md-2">
                        <label>Gerações:</label>
                        <input type="text" 
                            className="form-control"
                            value={this.props.location.generations}
                             onChange={this.props.setConfig('generations')} />
                    </div>
                    <div className="col-md-2">
                        <label>População:</label>
                        <input type="text" className="form-control"
                            value={this.props.location.population}
                         onChange={this.props.setConfig('population')} />
                    </div>
                </div>
                <div className="row">
                    <div className="col-sm-12">
                        <SortableList items={this.props.items} onSortEnd={this.onSortEnd} onRemove={this.onRemoveLocation} helperClass="SortableHelper" useDragHandle={true} />
                    </div>  
                </div>

            </div>
        )
    }
}
