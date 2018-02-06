import React, {Component} from 'react';
import {render} from 'react-dom';
import {SortableContainer, SortableElement, arrayMove} from 'react-sortable-hoc';

const SortableItem = SortableElement(({value}) => <li className="SortableItem">{value}</li>);
const SortableList = SortableContainer(({items}) => {
    return (
        <ul className="SortableList">
            {items.map((value, index) =>
                <SortableItem key={`item-${index}`} index={index} value={value} />
            )}
        </ul>
    );
});

export default class SortableComponent extends React.Component {
		constructor(props) {
        super(props);
        this.state = {
            items: Array.apply(null, Array(10)).map((val, index) => 'Item ' + index)
        }
    }
    onSortEnd({oldIndex, newIndex}) {
        this.setState({
            items: arrayMove(this.state.items, oldIndex, newIndex)
        });
    }
    render() {
        return (
            <SortableList items={this.state.items} onSortEnd={this.onSortEnd.bind(this)} helperClass="SortableHelper" />
        )
    }
}