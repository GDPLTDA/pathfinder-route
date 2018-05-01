import React from 'react'
import moment from 'moment';
import PlacesAutocomplete from 'react-places-autocomplete'
import TimePicker from 'rc-time-picker';
import NumericInput from 'react-numeric-input';

const now = moment();
const cssClasses = {
  root: 'form-group',
  input: 'form-control',
  autocompleteContainer: 'autocomplete-container'
}
const renderSuggestion =  ({ formattedSuggestion }) => (
  <div className="suggestion-item">
    <i className="fa fa-map-marker suggestion-icon" />
    <strong>{formattedSuggestion.mainText}</strong>{' '}
    <small className="text-muted">
      {formattedSuggestion.secondaryText}
    </small>
  </div>
)

  const PlaceSearch = (props) => {

    const inputProps = {
      value: props.address,
      onChange: props.onTextChange,
      placeholder: 'Endereços...'
    }
    
    return (
      <div className="form-group search-bar">
        <div className="row">
          <div className="col-sm-12">
            Procurar<PlacesAutocomplete
                    classNames={cssClasses} 
                    inputProps={inputProps} 
                    onSelect={props.onHandleSelect}
                    renderSuggestion={renderSuggestion} />
          </div>
        </div>
        <div className="row">
          <div className="col-sm-4">
              Abertura<TimePicker
                      showSecond={false}
                      className="form-control"
                      defaultValue={now}
                      onChange={props.onChangeFrom}
                      format={props.format}
                  />
          </div>
          <div className="col-sm-4">
              Fechamento<TimePicker
                      showSecond={false}
                      className="form-control"
                      defaultValue={now}
                      onChange={props.onChangeTo}
                      format={props.format}
                  />
          </div>
          <div className="col-sm-2">
              Entregadores<NumericInput 
                      mobile
                      className="form-control"
                      onChange={props.onChangeEntregador} 
                      min={1}
                      value={props.ValueEntregador}
                      />
          </div>
          <div className="col-sm-2">
              Desgarga(+/-)<NumericInput 
                    mobile
                    className="form-control"
                    onChange={props.onChangeWait} 
                    min={10}
                    value={props.ValueWait}
                    />
          </div>
        </div>
      </div>
    )
  }

  export default PlaceSearch