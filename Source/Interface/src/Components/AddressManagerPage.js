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
            center={{lat: props.lat,lng: props.lng}}
            markers={[{lat: props.lat,lng: props.lng}]}
            zoom={17}
            loadingElement={<div style={{ height: `100%` }} />}
            containerElement={<div style={{ height: `100%` }} />}
            mapElement={<div style={{ height: `100%` }} />}
            googleMapURL={"https://maps.googleapis.com/maps/api/js?key=AIzaSyC4R6AN7SmujjPUIGKdyao2Kqitzr1kiRg&v=3.exp&libraries=geometry,drawing,places"}
        />
    </div>
</div>

export default AddressManagerPage