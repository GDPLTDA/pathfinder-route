import React from 'react'

const LoadingSpinner = (props) =>
        <div>{ (props.mensagem !== null && props.mensagem !== "")  && <div className="col-sm-12 alert alert-danger" role="alert">
            {props.mensagem}
        </div>}</div>
        
export default LoadingSpinner