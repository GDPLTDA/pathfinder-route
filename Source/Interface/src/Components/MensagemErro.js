import React from 'react'

const LoadingSpinner = (props) =>
        <div className="col-sm-12">
            <span className="badge badge-danger">{props.mensagem}</span>
        </div>

export default LoadingSpinner