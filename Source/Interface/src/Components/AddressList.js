import React from 'react'
import _ from 'lodash'
import SortableList from './SortableList'


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
            <div>
                <div className="form-group">
                    <button className="btn btn-success" onClick={this.addLocation}>Adicionar</button>
                    <button className="btn btn-info" onClick={this.props.Teste}>Dados de Teste</button>
                </div>
                <div className="form-group">
                    <SortableList items={this.props.items} onSortEnd={this.onSortEnd} onRemove={this.onRemoveLocation} helperClass="SortableHelper" useDragHandle={true} />
                </div>  
            </div>
        )
    }
}
