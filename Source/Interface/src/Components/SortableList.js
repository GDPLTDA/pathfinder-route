import React from 'react';
import {SortableContainer, SortableElement, SortableHandle} from 'react-sortable-hoc';

const DragHandle = SortableHandle(() => <i className="fa fa-bars sort-item" />)

const SortableItem = SortableElement(
    ({location, onRemove}) => 
    <li className="SortableItem">
        <DragHandle />
        <div>
          {location.address} <br/>
          Abre as {location.from} e fecha as {location.to} e espera de {location.wait} Minutes<br/>
        </div>
        <div className="gps-point">
          <i className="fa fa-map-marker" />
          <span className={location.isStore ? "note noteStore" : "note"}>{location.isStore ? "Deposito" : "Destino"}  </span>
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