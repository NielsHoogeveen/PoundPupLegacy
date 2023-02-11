'use strict';
const path = require('path');
const { styles } = require('@ckeditor/ckeditor5-dev-utils');
module.exports = {
    mode: 'development',
    entry: './scripts/app.ts',
    output: {
        path: path.resolve(__dirname, 'wwwroot/js'),
        filename: 'editor.js',
        library: {
            name: 'Editor',
            type: "umd",
        },
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
            {
                test: /ckeditor5-[^/\\]+[/\\]theme[/\\]icons[/\\][^/\\]+\.svg$/,
                use: ['raw-loader']
            },
            {
                test: /ckeditor5-[^/\\]+[/\\]theme[/\\].+\.css$/,
                use: [
                    {
                        loader: 'style-loader',
                        options: {
                            injectType: 'singletonStyleTag',
                            attributes: {
                                'data-cke': true
                            }
                        }
                    },
                    'css-loader',
                    {
                        loader: 'postcss-loader',
                        options: {
                            postcssOptions: styles.getPostCssConfig({
                                themeImporter: {
                                    themePath: require.resolve('@ckeditor/ckeditor5-theme-lark')
                                },
                                minify: true
                            })
                        }
                    }
                ]
            }
        ]
    },
    // Useful for debugging.
    devtool: 'source-map',
    // By default webpack logs warnings if the bundle is bigger than 200kb.
    performance: { hints: false }
};
//# sourceMappingURL=webpack.config.js.map