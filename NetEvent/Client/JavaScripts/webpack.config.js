module.exports = {
    entry: {
        checkout: './src/checkout.js',
    },
    output: {
        filename: '[name].js',
        path: __dirname + '/../wwwroot/js',
    },
    module: {
        rules: [
            {
                test: /\.css$/i,
                use: ["style-loader", "css-loader"],
            },
        ],
    },
};