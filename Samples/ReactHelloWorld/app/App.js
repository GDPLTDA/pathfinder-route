import React from 'react'
import ReactDOM from 'react-dom'

import SearchComponent from './components/Search'

ReactDOM.render(<SearchComponent/>, document.getElementById('app'));

ReactDOM.render(
<SortableComponent />,
document.getElementById('list')
);