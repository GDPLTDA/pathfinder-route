import React from 'react'
import PlaceSearch from './PlaceSearch'
import AdressList from './AddressList'
import SearchRouteButton from './SearchRouteButton'
import Map from './Map'

const AddressManagerPage = (props) =>  
<div className="form-group row app">
    <div className="form-group col-sm-7">
        <PlaceSearch
            onSelect={props.onSelect}
            onHandleSelect={props.onHandleSelect}
            onChangeFrom={props.onChangeFrom}
            onChangeTo={props.onChangeTo}
            onChangeWait={props.onChangeWait}
            onChangeEntregador={props.onChangeEntregador}
            ValueWait={props.ValueWait}
            ValueEntregador={props.ValueEntregador}
            onTextChange={props.onTextChange}
            format={props.format}
            address={props.address}
        />
        <AdressList
            onRemoveLocation={props.onRemoveLocation}
            onSortEnd={props.onSortEnd}
            items={props.listLocations}
            location={props.location}
            setConfig={props.setConfig}
            onClickButton={props.onClickButton}
            SelectedOption={props.SelectedOption}
            SelectTestChange={props.SelectTestChange}
        />
        <SearchRouteButton Search={props.search} Label="Calcular Rota"/>
    </div>
    <div className="col-sm-5">
        <Map
            center={{lat: props.lat,lng: props.lng}}
            markers={[{lat: props.lat,lng: props.lng}]}
            zoom={17}
            loadingElement={<div style={{ height: `100%` }} />}
            containerElement={<div style={{ height: `100%` }} />}
            mapElement={<div style={{ height: `100%` }} />}
            googleMapURL={"https://maps.googleapis.com/maps/api/js?key=AIzaSyBm6unznpnoVDNak1s-iV_N9bQqCVpmKpE&v=3.exp&libraries=geometry,drawing,places"}
        />
    </div>
</div>

export default AddressManagerPage