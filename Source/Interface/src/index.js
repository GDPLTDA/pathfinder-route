import React from 'react';
import ReactDOM from 'react-dom';
import './App.css';
import App from './Components/App';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-bootstrap/dist/react-bootstrap.min.js';
import 'rc-time-picker/assets/index.css';
import 'toastr/build/toastr.min.css'
import 'react-table/react-table.css'

import { BrowserRouter, withRouter  } from "react-router-dom"
const AppWithRouter = withRouter(App)

ReactDOM.render((
    <BrowserRouter>
        <AppWithRouter />
    </BrowserRouter>
  ), document.getElementById('root'))
