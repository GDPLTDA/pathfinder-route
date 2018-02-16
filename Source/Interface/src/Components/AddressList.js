import React from 'react'
import _ from 'lodash'
import SortableList from './SortableList'
import {arrayMove} from 'react-sortable-hoc'

export default class AddressList extends React.Component
{
    constructor()
    { 
        super()
        this.state = {items : []}
    }

    addLocation = () => {
        const locations = this.state.items
        const newLocation = {...this.props.location}

        const found = locations.some( el =>
            el.lat === newLocation.lat 
            && el.lng === newLocation.lng
        )

        if ( newLocation.address==="" || found)
            return;

        if(locations.length != 0)
            newLocation.isStore = false

        locations.push(newLocation)
        this.setState({items: locations})

        this.props.onClickButton()
    }

    removeLocation = (item) => {
        const items = this.state.items
        _.remove(items, item)
        this.setState({items: this.updateStoreStatus(items)})
    }

    onSortEnd = ({oldIndex, newIndex}) => {
        this.setState({
            items: arrayMove(this.state.items, oldIndex, newIndex),
            });
         
        this.setState({items: this.updateStoreStatus(this.state.items)})
    };

    updateStoreStatus(locations = []){
        return locations.map(  
            (entry,index) =>
               (index == 0)
               ? {...entry, isStore: true}
               : {...entry, isStore: false}
            )
    }

    render() {
        return (
            <div>
                <div className="form-group">
                    <button className="btn btn-success" onClick={this.addLocation}>Add</button>
                </div>
                <div className="form-group">
                    <SortableList items={this.state.items} onSortEnd={this.onSortEnd} onRemove={this.removeLocation} helperClass="SortableHelper" useDragHandle={true} />
                </div>  
            </div>
        )
    }
}
