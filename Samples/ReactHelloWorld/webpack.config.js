module.exports = {
    entry: "./app/App.js",
    output: {
      filename: "public/bundle.js"
    },
    module: {
      loaders: [
        {
          test: /\.js$/,
          exclude: /node_modules/,
          loader: 'babel',
          loader: 'babel-loader',
          query: {
            "presets": [
              ["es2015", { "modules": false, "loose": true }],
              "react",
              "stage-0"
            ]
          }
        }
      ]
    }
  }