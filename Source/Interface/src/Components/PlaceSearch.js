import React from 'react'
import moment from 'moment';
import PlacesAutocomplete from 'react-places-autocomplete'
import TimePicker from 'rc-time-picker';

const now = moment().hour(0).minute(0);
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
      placeholder: 'Search Places...'
    }
    
    return (
        <div className="form-group">
					<label htmlFor="addressInput">Address</label>
          <PlacesAutocomplete
                classNames={cssClasses} 
                inputProps={inputProps} 
                onSelect={props.onHandleSelect}
                renderSuggestion={renderSuggestion} />
          <TimePicker
              showSecond={false}
              defaultValue={now}
              classNames={cssClasses}
              onChange={props.onChangeFrom}
              format={props.format}
          />
          <TimePicker
              showSecond={false}
              defaultValue={now}
              classNames={cssClasses}
              onChange={props.onChangeTo}
              format={props.format}
          />
        </div>
    )
  }

  export default PlaceSearch