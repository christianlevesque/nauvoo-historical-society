const { join } = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const TerserPlugin = require("terser-webpack-plugin");

const srcPath = join(__dirname, "wwwsrc");

module.exports = {
	target: "web",
	entry: {
		app: join(srcPath, "index.js")
	},
	output: {
		path: join(__dirname, "wwwroot"),
		filename: "js/[name].js",
		publicPath: "/"
	},
	plugins: [
		new MiniCssExtractPlugin({
			filename: "css/[name].css",
			ignoreOrder: false
		})
	],
	module: {
		rules: [
			{
				test: /\.js$/,
				exclude: /node_modules/,
				use: [ "babel-loader" ]
			},
			{
				test: /.scss$/,
				use: [
					MiniCssExtractPlugin.loader,
					"css-loader",
					{
						loader: "postcss-loader",
						options: {
							postcssOptions: {
								plugins: [ require("autoprefixer") ]
							}
						}
					},
					"sass-loader"
				]
			}
		]
	},
	resolve: {
		extensions: [ ".js" ],
		alias: {
			"@": srcPath
		}
	},
	watchOptions: {
		ignored: /node_modules/
	},
	optimization: {
		minimizer: [
			new TerserPlugin({
				extractComments: false,
				terserOptions: {
					format: {
						comments: false
					}
				}
			})
		]
	},
	devtool: "source-map",
	mode: "production"
};