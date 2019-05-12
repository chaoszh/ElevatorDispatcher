# 电梯调度系统说明文档
**Operation System Assignment1**
**郑超 1751747**
## 一、项目分析
### 项目背景
某一层楼20层，有五部互联的电梯，基于线程思想，编写一个电梯调度程序。
### 项目目的
- 学习调度算法
- 通过实现电梯调度，体会操作系统调度过程
- 学习特定环境下多线程方法
### 项目需求
- **电梯内部功能键与状态显示**：
  - 数字键、关门键、开门键、上行键、下行键、报警键、当前电梯的楼层数、上升及下降状态等。
- **电梯外部功能键与状态显示**：
  - 上行键、下行键、当前电梯状态的数码显示器。
- **其它**： 
  - 五部电梯外部按钮相互联结。（即：当一个电梯按钮按下去时，其他电梯的相应按钮也就同时点亮。）
  - 所有电梯初始状态都在第一层。
  - 每个电梯如果在它的上层或者下层没有相应请求情况下，在原地保持不动。
## 二、开发工具
- **开发环境**：Windows
- **开发工具**：Unity3D
- **开发语言**：C#
## 三、项目实现
### 调度算法设计
总体思路：

电梯运行原理：
min-max之间移动 直到任务=0