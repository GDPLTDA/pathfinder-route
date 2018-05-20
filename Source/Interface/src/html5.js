const getGeoLocation =  (options)  =>
   new Promise( (resolve, reject) =>
    navigator.geolocation.getCurrentPosition(resolve, reject, options))

const sleep = (ms) =>
     new Promise(resolve => setTimeout(resolve, ms));


export {  getGeoLocation, sleep } 