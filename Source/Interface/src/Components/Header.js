import React from 'react'
import { Link } from 'react-router-dom'


const Header = (props) => 
    <nav className="navbar navbar-expand-lg navbar-light bg-light">
    <a className="navbar-brand" href="#">Route Finder</a>
    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span className="navbar-toggler-icon"></span>
    </button>
    <div className="collapse navbar-collapse" id="navbarNav">
        <ul className="navbar-nav">
        <li className="nav-item active">
            <Link className="nav-link" to="/">Inicio<span className="sr-only">(current)</span></Link>
        </li>
        <li className="nav-item">
            <Link className={`nav-link ${props.hasResults ? "" : "disabled" }`} to="/result" onClick={e => !props.hasResults && e.preventDefault() } >Resultados</Link>
        </li>
        </ul>
    </div>
    </nav>

export default Header;