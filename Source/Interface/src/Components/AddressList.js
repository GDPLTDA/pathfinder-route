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
            <div>
                <div className="form-group">
                    <button className="btn btn-success" onClick={this.addLocation}>Adicionar</button>
                    <div>Testes <Select
                        name="form-field-name"
                        value={this.props.SelectedOption}
                        onChange={this.props.SelectTestChange}
                        options={[
                        { value: 0, label: 'Diversos' },
                        { value: 1, label: 'McDonald’s' },
                        { value: 2, label: 'Uninove' },
                        { value: 3, label: 'Extra' },
                        { value: 4, label: 'Senac' },
                        { value: 5, label: 'Turismo São Paulo' },
                        ]}
                        />
                    </div>
                </div>
                <div className="form-group">
                    <SortableList items={this.props.items} onSortEnd={this.onSortEnd} onRemove={this.onRemoveLocation} helperClass="SortableHelper" useDragHandle={true} />
                </div>  
            </div>
        )
    }
}
