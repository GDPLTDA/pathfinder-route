import React, {Component} from 'react';
import SortableComponent from './List'

export default class SearchComponent extends React.Component {
	render() {
		return (
			<div>{}
				<div className="form-group">
					<label htmlFor="addressInput">Address</label> {}
					<input id="addressInput" className="form-control" type="text" placeholder="Enter a location"/>

				</div>
				<div className="form-group">
					<button type="button" className="btn btn-primary">Add</button> {}
				</div>
				<SortableComponent/>
			</div>
		);
	}
}