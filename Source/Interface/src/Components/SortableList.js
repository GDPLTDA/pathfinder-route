import React from 'react';
import {SortableContainer, SortableElement, SortableHandle} from 'react-sortable-hoc';

const DragHandle = SortableHandle(() => <i className="fa fa-bars sort-item" />)

const SortableItem = SortableElement(
    ({location, onRemove}) => 
    <li className="SortableItem">
        <DragHandle />
        <div>
          {location.from} - {location.to} <br/>
          {location.address} 
        </div>
        <div className="gps-point">
          <i className="fa fa-map-marker" />
          <span className={location.isStore ? "note noteStore" : "note"}>{location.isStore ? "Store" : "Place"}  </span>
          <span className="note">{location.lat.toFixed(5)}, {location.lng.toFixed(5)}</span>
        </div>
        <button className="remove-button" onClick={e => onRemove(location)}>
          <i className="fa fa-trash"/>
        </button>
    </li>
)

const SortableList = SortableContainer(({items, onRemove}) => {
  return (
    <ul className="SortableList">
      {items.map((value, index) => (
        <SortableItem key={`item-${index}`} index={index} location={value} onRemove={onRemove} />
      ))}
    </ul>
  );
});

export default SortableList