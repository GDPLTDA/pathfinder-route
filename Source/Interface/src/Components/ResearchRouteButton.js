import React from 'react'

export default class ResearchRouteButton extends React.Component {

    Research = () => {
        this.props.Research(this.props.Index,this.props.Rotas, this.props.Time())
    }

    render() {
        return (
            <div>
                <div className="form-group">
                    <button 
                        className="btn btn-success btn-block" 
                        onClick={this.Research}>
                        {this.props.Label}
                    </button>
                </div> 
            </div>
    )
  }
}