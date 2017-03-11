$(function(){

	var myChart = echarts.init(document.getElementById('chart-1'));

	function randomData() {
		now = new Date(+now + oneDay);
		value = value + Math.random() * 21 - 10;
		return {
			name: now.toString(),
			value: [
				[now.getFullYear(), now.getMonth() + 1, now.getDate()].join('/'),
				Math.round(value)
			]
		}
	}

	var data = [];
	var now = +new Date(1997, 9, 3);
	var oneDay = 24 * 3600 * 1000;
	var value = Math.random() * 1000;
	for (var i = 0; i < 1000; i++) {
		data.push(randomData());
	}

	var option = {
		// title: {
		//     text: '动态数据 + 时间坐标轴'
		// },
		tooltip: {
			trigger: 'axis',
			formatter: function(params) {
				params = params[0];
				var date = new Date(params.name);
				return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear() + ' : ' + params.value[1];
			},
			axisPointer: {
				animation: false
			}
		},
		xAxis: {
			type: 'time',
			splitLine: {
				show: false
			}
		},
		yAxis: {
			type: 'value',
			boundaryGap: [0, '100%'],
			splitLine: {
				show: false
			}
		},
		series: [{
			name: '模拟数据',
			type: 'line',
			showSymbol: false,
			hoverAnimation: false,
			data: data
		}],
		color: ['#00ba99'],
	};

	myChart.setOption(option);

	var timeTicket = setInterval(function() {

		for (var i = 0; i < 5; i++) {
			data.shift();
			data.push(randomData());
		}

		myChart.setOption({
			series: [{
				data: data
			}]
		});
	}, 1000);
			
});
$(function(){
	var myChart = echarts.init(document.getElementById('chart-2'));
		var option = {
			// title: {
			//     // text: '堆叠区域图'
			// },
			tooltip: {
				trigger: 'axis'
			},
			legend: {
				data: ['实时数据', '平均数据']
			},
			toolbox: {
				feature: {
					saveAsImage: {}
				}
			},
			grid: {
				left: '3%',
				right: '4%',
				bottom: '3%',
				containLabel: true
			},
			xAxis: [{
				type: 'category',
				boundaryGap: false,
				data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
			}],
			yAxis: [{
				type: 'value'
			}],
			series: [{
				name: '实时数据',
				type: 'line',
				stack: '总量',
				areaStyle: {
					normal: {}
				},
				data: [120, 132, 101, 134, 90, 230, 210]
			}, {
				name: '平均数据',
				type: 'line',
				stack: '总量',
				areaStyle: {
					normal: {}
				},
				data: [220, 182, 191, 234, 290, 330, 310]
			}, ],
			color: ['#3a8eff', '#00ba99'],
		};

		myChart.setOption(option);
});

