import React from 'react'
import moment from 'moment';
import PlacesAutocomplete, { geocodeByAddress, getLatLng } from 'react-places-autocomplete'
import TimePicker from 'rc-time-picker';

const format = 'HH:mm';
const now = moment().hour(0).minute(0);

export default class PlaceSearch extends React.Component {
  constructor(props) {
    super(props)
    this.state = { address: '', lat:0, lng:0, isStore: true, from:'00:00', to:'00:00'}
  }

  onChange = (address) => this.setState({ address })
  
  handleSelect = async (address) => {
    this.setState({ address })

    let latLng = await geocodeByAddress(this.state.address)
                      .then(results => getLatLng(results[0]));
    
     this.setState({...latLng})
     this.props.onSelect(this.state)
  }
  onChangeFrom = (value) => {
    const from = value.format(format)
    this.setState({ from })

    this.props.onSelect(this.state)
    console.log(value && from);
  }
  onChangeTo = (value) => {
    const to = value.format(format)
    this.setState({ to })

    this.props.onSelect(this.state)
    console.log(value && to);
  }
  render() {

    const inputProps = {
      value: this.state.address,
      onChange: this.onChange,
      placeholder: 'Search Places...'
    }

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
    
    return (
        <div className="form-group">
					<label htmlFor="addressInput">Address</label>
          <PlacesAutocomplete
                classNames={cssClasses} 
                inputProps={inputProps} 
                onSelect={this.handleSelect}
                renderSuggestion={renderSuggestion} />
          <TimePicker
              showSecond={false}
              defaultValue={now}
              classNames={cssClasses}
              onChange={this.onChangeFrom}
              format={format}
          />
          <TimePicker
              showSecond={false}
              defaultValue={now}
              classNames={cssClasses}
              onChange={this.onChangeTo}
              format={format}
          />
        </div>
    )
  }
}
