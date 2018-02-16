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

    addLocation = (e) => {
        //e.preventDefault()
        const locations = this.state.items
        const newLocation = {...this.props.location}

        var found = locations.some(function (el) {
            return el.lat === newLocation.lat && el.lng === newLocation.lng;
          });

        if ( newLocation.address==="" || found)
            return;

        if(locations.length != 0)
            newLocation.store = "Place"

        locations.push(newLocation)
        this.setState({items: locations})

        this.props.onClickButton()
    }

    removeLocation = (item) => {
        const items = this.state.items
        _.remove(items, item)
        this.setState({items})
    }

    onSortEnd = ({oldIndex, newIndex}) => {
        this.setState({
            items: arrayMove(this.state.items, oldIndex, newIndex),
            });

        const locations = this.state.items
        locations.forEach(function(entry) {
            if(locations.indexOf(entry) == 0)
                entry.store = "Store"
            else
                entry.store = "Place"
            console.log(entry);
        });
        this.setState({items: locations})
    };

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
