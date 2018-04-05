import React from 'react'
import PlaceSearch from './PlaceSearch'
import AdressList from './AddressList'
import SearchRouteButton from './SearchRouteButton'
import Map from './Map'

const AddressManagerPage = (props) =>  
<div className="form-group row app">
    <div className="form-group col-sm-5">
        <PlaceSearch
            onSelect={props.onSelect}
            onHandleSelect={props.onHandleSelect}
            onChangeFrom={props.onChangeFrom}
            onChangeTo={props.onChangeTo}
            onChangeWait={props.onChangeWait}
            ValueWait={props.valueWait}
            onTextChange={props.onTextChange}
            format={props.format}
            address={props.address}
        />
        <AdressList
            onRemoveLocation={props.onRemoveLocation}
            onSortEnd={props.onSortEnd}
            items={props.listLocations}
            location={props.location}
            onClickButton={props.onClickButton}
            Teste={props.getDados}
        />
        <SearchRouteButton Search={props.search}/>
    </div>
    <div className="col-sm-7">
        <Map
            lat={props.lat}
            lng={props.lng}
            loadingElement={<div style={{ height: `100%` }} />}
            containerElement={<div style={{ height: `100%` }} />}
            mapElement={<div style={{ height: `100%` }} />}
        />
    </div>
</div>

export default AddressManagerPage